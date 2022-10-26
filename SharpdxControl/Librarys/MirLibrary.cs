using SharpDX.Direct3D9;
using SharpDX;
using SharpdxControl.Enums;
using SharpdxControl.Envir;
using System.IO;
using Library;
using SharpdxControl.SharpDXs;

namespace SharpdxControl.Librarys
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/26 10:45:14 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public sealed class MirLibrary : IDisposable
    {
        public readonly object LoadLocker = new object();//锁
        public string FileName;//文件名

        private FileStream _FStream;//文件流
        private BinaryReader _BReader;//读取流

        public bool Loaded, Loading;

        public MirImage[] Images;//图片集合

        public MirLibrary(string fileName)
        {
            _FStream = File.OpenRead(fileName);
            _BReader = new BinaryReader(_FStream);
        }

        public void ReadLibrary()
        {
            lock (LoadLocker)
            {
                if (Loading) return;
                Loading = true;
            }

            if (_BReader == null)
            {
                Loaded = true;
                return;
            }

            using (MemoryStream mstream = new MemoryStream(_BReader.ReadBytes(_BReader.ReadInt32())))
            using (BinaryReader reader = new BinaryReader(mstream))
            {
                Images = new MirImage[reader.ReadInt32()];

                for (int i = 0; i < Images.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;

                    Images[i] = new MirImage(reader);
                }
            }


            Loaded = true;
        }

        /// <summary>
        /// 获取图片大小
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Size GetSize(int index)
        {
            if (!CheckImage(index)) return Size.Empty;

            return new Size(Images[index].Width, Images[index].Height);
        }
        /// <summary>
        /// 获取图片偏移
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public System.Drawing.Point GetOffSet(int index)
        {
            if (!CheckImage(index)) return System.Drawing.Point.Empty;

            return new System.Drawing.Point(Images[index].OffSetX, Images[index].OffSetY);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MirImage GetImage(int index)
        {
            if (!CheckImage(index)) return null;

            return Images[index];
        }
        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public MirImage CreateImage(int index, ImageType type)
        {
            if (!CheckImage(index)) return null;

            MirImage image = Images[index];

            Texture texture;

            switch (type)
            {
                case ImageType.Image:
                    if (!image.ImageValid) image.CreateImage(_BReader);
                    texture = image.Image;
                    break;
                case ImageType.Shadow:
                    if (!image.ShadowValid) image.CreateShadow(_BReader);
                    texture = image.Shadow;
                    break;
                case ImageType.Overlay:
                    if (!image.OverlayValid) image.CreateOverlay(_BReader);
                    texture = image.Overlay;
                    break;
                default:
                    return null;
            }

            if (texture == null) return null;

            return image;
        }

        /// <summary>
        /// 判断图片存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckImage(int index)
        {
            if (!Loaded) ReadLibrary();

            while (!Loaded)
                Thread.Sleep(1);

            return index >= 0 && index < Images.Length && Images[index] != null;
        }

        public bool VisiblePixel(int index,System.Drawing.Point location, bool accurate = true, bool offSet = false)
        {
            if (!CheckImage(index)) return false;

            MirImage image = Images[index];

            if (offSet)
                location = new System.Drawing.Point(location.X - image.OffSetX, location.Y - image.OffSetY);

            return image.VisiblePixel(location, accurate);
        }

        public void Draw(int index, float x, float y, System.Drawing.Color colour,System.Drawing.Rectangle area, float opacity, ImageType type, byte shadow = 0)
        {
            if (!CheckImage(index)) return;

            MirImage image = Images[index];

            Texture texture;

            float oldOpacity = DXManager.Opacity;
            switch (type)
            {
                case ImageType.Image:
                    if (!image.ImageValid) image.CreateImage(_BReader);
                    texture = image.Image;
                    break;
                case ImageType.Shadow:
                    if (!image.ShadowValid) image.CreateShadow(_BReader);
                    texture = image.Shadow;

                    if (texture == null)
                    {
                        if (!image.ImageValid) image.CreateImage(_BReader);
                        texture = image.Image;

                        switch (image.ShadowType)
                        {
                            case 177:
                            case 176:
                            case 49:
                                Matrix m = Matrix.Scaling(1F, 0.5f, 0);

                                m.M21 = -0.50F;
                                DXManager.Sprite.Transform = m * Matrix.Translation(x + image.Height / 2, y, 0);

                                DXManager.Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.None);
                                if (oldOpacity != 0.5F) DXManager.SetOpacity(0.5F);

                                DXManager.Sprite.Draw(texture, Vector3.Zero, Vector3.Zero, System.Drawing.Color.Black);
                                CEnvir.DPSCounter++;

                                DXManager.SetOpacity(oldOpacity);
                                DXManager.Sprite.Transform = Matrix.Identity;
                                DXManager.Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);

                                image.ExpireTime = Time.Now + Config.CacheDuration;
                                break;
                            case 50:
                                if (oldOpacity != 0.5F) DXManager.SetOpacity(0.5F);

                                DXManager.Sprite.Draw(texture, Vector3.Zero, new Vector3(x, y, 0), System.Drawing.Color.Black);
                                CEnvir.DPSCounter++;
                                DXManager.SetOpacity(oldOpacity);

                                image.ExpireTime = Time.Now + Config.CacheDuration;
                                break;
                        }
                        return;
                    }
                    break;
                case ImageType.Overlay:
                    if (!image.OverlayValid) image.CreateOverlay(_BReader);
                    texture = image.Overlay;
                    break;
                default:
                    return;
            }

            if (texture == null) return;

            DXManager.SetOpacity(opacity);

            DXManager.Sprite.Draw(texture, area, Vector3.Zero, new Vector3(x, y, 0), colour);
            CEnvir.DPSCounter++;
            DXManager.SetOpacity(oldOpacity);

            image.ExpireTime = Time.Now + Config.CacheDuration;
        }
        public void Draw(int index, float x, float y, System.Drawing.Color colour, bool useOffSet, float opacity, ImageType type, float scale = 1F)
        {
            if (!CheckImage(index)) return;

            MirImage image = Images[index];

            Texture texture;

            Matrix scaling, rotationZ, translation;

            float oldOpacity = DXManager.Opacity;
            switch (type)
            {
                case ImageType.Image:
                    if (!image.ImageValid) image.CreateImage(_BReader);
                    texture = image.Image;
                    if (useOffSet)
                    {
                        x += image.OffSetX;
                        y += image.OffSetY;
                    }
                    break;
                case ImageType.Shadow:
                    {
                        if (!image.ShadowValid) image.CreateShadow(_BReader);
                        texture = image.Shadow;

                        if (useOffSet)
                        {
                            x += image.ShadowOffSetX;
                            y += image.ShadowOffSetY;
                        }

                        if (texture == null)
                        {
                            if (!image.ImageValid) image.CreateImage(_BReader);
                            texture = image.Image;

                            switch (image.ShadowType)
                            {
                                case 177:
                                case 176:
                                case 49:
                                    Matrix m = Matrix.Scaling(1F * scale, 0.5f * scale, 0);

                                    m.M21 = -0.50F;
                                    DXManager.Sprite.Transform = m * Matrix.Translation(x + image.Height / 2, y, 0);

                                    DXManager.Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.None);
                                    if (oldOpacity != 0.5F) DXManager.SetOpacity(0.5F);

                                    DXManager.Sprite.Draw(texture, Vector3.Zero, Vector3.Zero, System.Drawing.Color.Black);
                                    CEnvir.DPSCounter++;

                                    DXManager.SetOpacity(oldOpacity);
                                    DXManager.Sprite.Transform = Matrix.Identity;
                                    DXManager.Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);

                                    image.ExpireTime = Time.Now + Config.CacheDuration;
                                    break;
                                case 50:
                                    if (oldOpacity != 0.5F) DXManager.SetOpacity(0.5F);

                                    scaling = Matrix.Scaling(scale, scale, 0f);
                                    rotationZ = Matrix.RotationZ(0F);
                                    translation = Matrix.Translation(x + (image.Width / 2), y + (image.Height / 2), 0);

                                    DXManager.Sprite.Transform = scaling * rotationZ * translation;

                                    DXManager.Sprite.Draw(texture, Vector3.Zero, new Vector3((image.Width / 2) * -1, (image.Height / 2) * -1, 0), System.Drawing.Color.Black);

                                    CEnvir.DPSCounter++;
                                    DXManager.SetOpacity(oldOpacity);

                                    image.ExpireTime = Time.Now + Config.CacheDuration;
                                    break;
                            }

                            return;
                        }
                    }
                    break;
                case ImageType.Overlay:
                    if (!image.OverlayValid) image.CreateOverlay(_BReader);
                    texture = image.Overlay;

                    if (useOffSet)
                    {
                        x += image.OffSetX;
                        y += image.OffSetY;
                    }
                    break;
                default:
                    return;
            }

            if (texture == null) return;

            scaling = Matrix.Scaling(scale, scale, 0f);
            rotationZ = Matrix.RotationZ(0F);
            translation = Matrix.Translation(x + (image.Width / 2), y + (image.Height / 2), 0);

            DXManager.SetOpacity(opacity);

            DXManager.Sprite.Transform = scaling * rotationZ * translation;

            DXManager.Sprite.Draw(texture, Vector3.Zero, new Vector3((image.Width / 2) * -1, (image.Height / 2) * -1, 0), colour);

            DXManager.Sprite.Transform = Matrix.Identity;

            CEnvir.DPSCounter++;

            DXManager.SetOpacity(oldOpacity);

            image.ExpireTime = Time.Now + Config.CacheDuration;
        }
        public void DrawBlend(int index, float size, System.Drawing.Color colour, float x, float y, float angle, float opacity, ImageType type, bool useOffSet = false, byte shadow = 0)
        {
            if (!CheckImage(index)) return;

            MirImage image = Images[index];

            Texture texture;

            switch (type)
            {
                case ImageType.Image:
                    if (!image.ImageValid) image.CreateImage(_BReader);
                    texture = image.Image;
                    if (useOffSet)
                    {
                        x += image.OffSetX;
                        y += image.OffSetY;
                    }
                    break;
                case ImageType.Shadow:
                    return;
                /*     if (!image.ShadowValid) image.CreateShadow(_BReader);
                     texture = image.Shadow;

                     if (useOffSet)
                     {
                         x += image.ShadowOffSetX;
                         y += image.ShadowOffSetY;
                     }
                     break;*/
                case ImageType.Overlay:
                    if (!image.OverlayValid) image.CreateOverlay(_BReader);
                    texture = image.Overlay;

                    if (useOffSet)
                    {
                        x += image.OffSetX;
                        y += image.OffSetY;
                    }
                    break;
                default:
                    return;
            }
            if (texture == null) return;


            bool oldBlend = DXManager.Blending;
            float oldRate = DXManager.BlendRate;

            DXManager.SetBlend(true, opacity);

            var scaling = Matrix.Scaling(size, size, 0f);
            var rotationZ = Matrix.RotationZ(angle);
            var translation = Matrix.Translation(x + (image.Width / 2), y + (image.Height / 2), 0);

            DXManager.Sprite.Transform = scaling * rotationZ * translation;

            DXManager.Sprite.Draw(texture, Vector3.Zero, new Vector3((image.Width / 2) * -1, (image.Height / 2) * -1, 0), colour);

            DXManager.Sprite.Transform = Matrix.Identity;

            CEnvir.DPSCounter++;

            DXManager.SetBlend(oldBlend, oldRate);

            image.ExpireTime = Time.Now + Config.CacheDuration;
        }
        public void DrawBlend(int index, float x, float y, System.Drawing.Color colour, bool useOffSet, float rate, ImageType type, byte shadow = 0)
        {
            if (!CheckImage(index)) return;

            MirImage image = Images[index];

            Texture texture;

            switch (type)
            {
                case ImageType.Image:
                    if (!image.ImageValid) image.CreateImage(_BReader);
                    texture = image.Image;
                    if (useOffSet)
                    {
                        x += image.OffSetX;
                        y += image.OffSetY;
                    }
                    break;
                case ImageType.Shadow:
                    return;
                /*     if (!image.ShadowValid) image.CreateShadow(_BReader);
                     texture = image.Shadow;

                     if (useOffSet)
                     {
                         x += image.ShadowOffSetX;
                         y += image.ShadowOffSetY;
                     }
                     break;*/
                case ImageType.Overlay:
                    if (!image.OverlayValid) image.CreateOverlay(_BReader);
                    texture = image.Overlay;

                    if (useOffSet)
                    {
                        x += image.OffSetX;
                        y += image.OffSetY;
                    }
                    break;
                default:
                    return;
            }
            if (texture == null) return;


            bool oldBlend = DXManager.Blending;
            float oldRate = DXManager.BlendRate;

            DXManager.SetBlend(true, rate);

            DXManager.Sprite.Draw(texture, Vector3.Zero, new Vector3(x, y, 0), colour);
            CEnvir.DPSCounter++;

            DXManager.SetBlend(oldBlend, oldRate);

            image.ExpireTime = Time.Now + Config.CacheDuration;
        }


        #region IDisposable Support

        public bool IsDisposed { get; private set; }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsDisposed = true;

                foreach (MirImage image in Images)
                    image.Dispose();


                Images = null;


                _FStream?.Dispose();
                _FStream = null;

                _BReader?.Dispose();
                _BReader = null;

                Loading = false;
                Loaded = false;
            }

        }

        ~MirLibrary()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
