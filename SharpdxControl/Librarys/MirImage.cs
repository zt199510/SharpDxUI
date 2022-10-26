using SharpDX;
using SharpDX.Direct3D9;
using SharpdxControl.Envir;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Librarys
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/26 10:46:15 
    /// Description   ：  图片信息
    ///********************************************/
    /// </summary>
    public sealed class MirImage : IDisposable
    {
        public int Position;

        #region Texture

        public short Width;
        public short Height;
        public short OffSetX;
        public short OffSetY;
        public byte ShadowType;
        public Texture Image;
        public bool ImageValid { get; private set; }
        public unsafe byte* ImageData;
        public int ImageDataSize
        {
            get
            {
                int w = Width + (4 - Width % 4) % 4;
                int h = Height + (4 - Height % 4) % 4;

                return w * h / 2;
            }
        }
        #endregion

        #region Shadow
        public short ShadowWidth;
        public short ShadowHeight;

        public short ShadowOffSetX;
        public short ShadowOffSetY;

        public Texture Shadow;
        public bool ShadowValid { get; private set; }
        public unsafe byte* ShadowData;
        public int ShadowDataSize
        {
            get
            {
                int w = ShadowWidth + (4 - ShadowWidth % 4) % 4;
                int h = ShadowHeight + (4 - ShadowHeight % 4) % 4;

                return w * h / 2;
            }
        }
        #endregion

        #region Overlay
        public short OverlayWidth;
        public short OverlayHeight;

        public Texture Overlay;
        public bool OverlayValid { get; private set; }
        public unsafe byte* OverlayData;
        public int OverlayDataSize
        {
            get
            {
                int w = OverlayWidth + (4 - OverlayWidth % 4) % 4;
                int h = OverlayHeight + (4 - OverlayHeight % 4) % 4;

                return w * h / 2;
            }
        }
        #endregion


        public DateTime ExpireTime;

        public MirImage(BinaryReader reader)
        {
            Position = reader.ReadInt32();

            Width = reader.ReadInt16();
            Height = reader.ReadInt16();
            OffSetX = reader.ReadInt16();
            OffSetY = reader.ReadInt16();

            ShadowType = reader.ReadByte();
            ShadowWidth = reader.ReadInt16();
            ShadowHeight = reader.ReadInt16();
            ShadowOffSetX = reader.ReadInt16();
            ShadowOffSetY = reader.ReadInt16();

            OverlayWidth = reader.ReadInt16();
            OverlayHeight = reader.ReadInt16();


        }

        public unsafe bool VisiblePixel(System.Drawing.Point p, bool acurrate)
        {
            if (p.X < 0 || p.Y < 0 || !ImageValid || ImageData == null) return false;

            int w = Width + (4 - Width % 4) % 4;
            int h = Height + (4 - Height % 4) % 4;

            if (p.X >= w || p.Y >= h)
                return false;

            int x = (p.X - p.X % 4) / 4;
            int y = (p.Y - p.Y % 4) / 4;
            int index = (y * (w / 4) + x) * 8;

            int col0 = ImageData[index + 1] << 8 | ImageData[index], col1 = ImageData[index + 3] << 8 | ImageData[index + 2];

            if (col0 == 0 && col1 == 0) return false;

            if (!acurrate || col1 < col0) return true;

            x = p.X % 4;
            y = p.Y % 4;
            x *= 2;

            return (ImageData[index + 4 + y] & 1 << x) >> x != 1 || (ImageData[index + 4 + y] & 1 << x + 1) >> x + 1 != 1;
        }


        public unsafe void DisposeTexture()
        {
            if (Image != null && !Image.IsDisposed)
                Image.Dispose();

            if (Shadow != null && !Shadow.IsDisposed)
                Shadow.Dispose();

            if (Overlay != null && !Overlay.IsDisposed)
                Overlay.Dispose();

            ImageData = null;
            ShadowData = null;
            OverlayData = null;

            Image = null;
            Shadow = null;
            Overlay = null;

            ImageValid = false;
            ShadowValid = false;
            OverlayValid = false;

            ExpireTime = DateTime.MinValue;

            DXManager.TextureList.Remove(this);
        }

        public unsafe void CreateImage(BinaryReader reader)
        {
            if (Position == 0) return;

            int w = Width + (4 - Width % 4) % 4;
            int h = Height + (4 - Height % 4) % 4;

            if (w == 0 || h == 0) return;

            Image = new Texture(DXManager.Device, w, h, 1, Usage.None, Format.Dxt1, Pool.Managed);
            DataRectangle rect = Image.LockRectangle(0, LockFlags.Discard);
            ImageData = (byte*)(void*)rect.DataPointer;
            byte[] array = null;
            lock (reader)
            {
                reader.BaseStream.Seek(Position, SeekOrigin.Begin);
                array = WriteDatapointer(reader.ReadBytes(ImageDataSize));
            }
            DataStream dataStream = new DataStream(rect.DataPointer, array.Length, true, true);
            dataStream.Write(array, 0, array.Length);
            Image.UnlockRectangle(0);
            dataStream.Dispose();
            array = null;
            ImageValid = true;
            ExpireTime = CEnvir.Now + Config.CacheDuration;
            DXManager.TextureList.Add(this);
        }
     

        public unsafe void CreateShadow(BinaryReader reader)
        {
            if (Position == 0) return;

            if (!ImageValid)
                CreateImage(reader);

            int w = ShadowWidth + (4 - ShadowWidth % 4) % 4;
            int h = ShadowHeight + (4 - ShadowHeight % 4) % 4;

            if (w == 0 || h == 0) return;

            Shadow = new Texture(DXManager.Device, w, h, 1, Usage.None, Format.Dxt5, Pool.Managed);
            DataRectangle rect = Shadow.LockRectangle(0, LockFlags.Discard);
            ShadowData = (byte*)(void*)rect.DataPointer;
            byte[] array = null;
            lock (reader)
            {
                reader.BaseStream.Seek(Position, SeekOrigin.Begin);
                array = WriteDatapointer(reader.ReadBytes(ShadowDataSize));
            }
            DataStream dataStream = new DataStream(rect.DataPointer, array.Length, true, true);
            dataStream.Write(array, 0, array.Length);
            Image.UnlockRectangle(0);
            dataStream.Dispose();
            array = null;

            ShadowValid = true;
        }
        public unsafe void CreateOverlay(BinaryReader reader)
        {
            if (Position == 0) return;

            if (!ImageValid)
                CreateImage(reader);

            int w = OverlayWidth + (4 - OverlayWidth % 4) % 4;
            int h = OverlayHeight + (4 - OverlayHeight % 4) % 4;

            if (w == 0 || h == 0) return;

            Overlay = new Texture(DXManager.Device, w, h, 1, Usage.None, Format.Dxt1, Pool.Managed);
            DataRectangle rect = Overlay.LockRectangle(0, LockFlags.Discard);
            OverlayData = (byte*)(void*)rect.DataPointer;
            byte[] array = null;
            lock (reader)
            {
                reader.BaseStream.Seek(Position, SeekOrigin.Begin);
                array = WriteDatapointer(reader.ReadBytes(OverlayDataSize));
            }
            DataStream dataStream = new DataStream(rect.DataPointer, array.Length, true, true);
            dataStream.Write(array, 0, array.Length);
            Image.UnlockRectangle(0);
            dataStream.Dispose();
            array = null;
            OverlayValid = true;
        }

        private static byte[] WriteDatapointer(byte[] Bytes)
        {
            using (DeflateStream deflateStream = new DeflateStream(new MemoryStream(Bytes), CompressionMode.Decompress))
            {
                byte[] buffer = new byte[4096];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int num = 0;
                    do
                    {
                        num = deflateStream.Read(buffer, 0, 4096);
                        if (num > 0)
                            memoryStream.Write(buffer, 0, num);
                    }
                    while (num > 0);
                    return memoryStream.ToArray();
                }
            }
        }
        #region IDisposable Support

        public bool IsDisposed { get; private set; }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsDisposed = true;

                Position = 0;

                Width = 0;
                Height = 0;
                OffSetX = 0;
                OffSetY = 0;

                ShadowWidth = 0;
                ShadowHeight = 0;
                ShadowOffSetX = 0;
                ShadowOffSetY = 0;

                OverlayWidth = 0;
                OverlayHeight = 0;
            }

        }

        public void Dispose()
        {
            Dispose(!IsDisposed);
            GC.SuppressFinalize(this);
        }
        ~MirImage()
        {
            Dispose(false);
        }

        #endregion

    }
}
