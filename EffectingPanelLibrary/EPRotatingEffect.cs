using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EffectingPanelLibrary
{
    public class EPRotatingEffect : EPDefaultEffect
    {
        public override void DrawEffectImage(Bitmap current, Bitmap next, EffectingPanel effecingPanel)
        {
            Bitmap doubleBufferingBitmap = null;    // ダブルバッファリング用画面
            Graphics bg = null;                     // ダブルバッファリング用画面描画用Graphics

            SolidBrush solidBrush = null;
            Rectangle rectangle;
            Matrix matrix = null;

            try
            {                
                int deltaDegree = 10;
                
                doubleBufferingBitmap = new Bitmap(current);
                bg = Graphics.FromImage(doubleBufferingBitmap);

                solidBrush = new SolidBrush(System.Drawing.Color.Black);
                rectangle = new Rectangle(0, 0, current.Width, current.Height);

                for (int angle = 0; angle <= 360; angle += deltaDegree)
                {
                    bg.ResetTransform();                             // リセット座標変換
                    bg.FillRectangle(solidBrush, rectangle);

                    matrix = new Matrix();
                    matrix.Translate(doubleBufferingBitmap.Width / 2, doubleBufferingBitmap.Height / 2, MatrixOrder.Append);   // 原点移動
                    matrix.Rotate((float)angle);
                    bg.Transform = matrix;                           // 座標設定

                    bg.DrawImage(current, -doubleBufferingBitmap.Width / 2, -doubleBufferingBitmap.Height / 2);  // 画像の中心が(0, 0)になるように描画
                    effecingPanel.pictureBox1.Image = doubleBufferingBitmap;
                    effecingPanel.pictureBox1.Refresh();

                    Thread.Sleep(20);
                    matrix.Dispose();
                }

                bg.Dispose();
                doubleBufferingBitmap.Dispose();
            }
            catch(SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
