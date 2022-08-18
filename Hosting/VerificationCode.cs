using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace SixLabors.ImageSharp
{
    public static class VerificationCode
    {
        private readonly static Font _font;
        static VerificationCode()
        {
            var collection = new FontCollection();
            FontFamily family = collection.Add("./Fronts/SIMKAI.TTF");
            _font = family.CreateFont(25, FontStyle.Italic);
        }
    
        public static string Generate(Stream stream)
        {
            var codes = new List<char>();
            using (var image = new Image<Rgba32>(100, 40, Rgba32.ParseHex("#ede4e4")))
            {
                image.Mutate(x =>
                {
                    int p = 28;
                    for (int i = 0; i < 4; i++)
                    {
                        var code = RandCode();
                        codes.Add(code[0]);
                        var matrix = Matrix3x2.CreateRotation(RandRadian(), new Vector2(p, 8));
                        x.SetDrawingTransform(matrix);
                        //旋转随机弧度，绘制验证码
                        x.DrawText(code, _font, RandCololr(), new PointF(p, 8));
                        p += 15;
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        //绘制干扰线
                        x.DrawLines(RandCololrA(), 1, RandPointF(), RandPointF());
                    }
                    for (int i = 0; i < 40; i++)
                    {
                        //绘制干扰点
                        x.Draw(RandCololrA(), 2, new RectangleF(RandPointF(), new SizeF(2, 2)));
                    }
                });
                image.SaveAsPng(stream);
            }
            return string.Join(string.Empty, codes);
        }
        private static Color RandCololr()
        {
            var r = (byte)Random.Shared.Next(0, 256);
            var g = (byte)Random.Shared.Next(0, 256);
            var b = (byte)Random.Shared.Next(0, 256);
            return Color.FromRgb(r, g, b);
        }
        private static Color RandCololrA()
        {
            var r = (byte)Random.Shared.Next(0, 256);
            var g = (byte)Random.Shared.Next(0, 256);
            var b = (byte)Random.Shared.Next(0, 256);
            var a = (byte)Random.Shared.Next(10, 80);
            return Color.FromRgba(r, g, b, a);
        }
        private static float RandRadian()
        {
            var radian = Random.Shared.Next(0, 50);
            return (float)(radian * Math.PI) / 180f;
        }
        private static PointF RandPointF()
        {
            var x = Random.Shared.Next(0, 100);
            var y = Random.Shared.Next(0, 40);
            return new PointF(x, y);
        }
        private static string RandCode()
        {
            var seeds = new List<char>();
            for (int i = 0; i <= 9; i++)
            {
                seeds.Add((char)(i + 48));
            }
            for (int i = 'A'; i <= 'Z'; i++)
            {
                seeds.Add((char)i);
                seeds.Add((char)(i + 32));
            }
            return seeds[Random.Shared.Next(0, seeds.Count)].ToString();
        }
    }
}
