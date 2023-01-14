using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jetpac
{
    public class Ledge : BaseObject
    {
        public Ledge(int x, int y, Texture2D image)
        {
            m_image = image;
            m_height = image.Height;
            m_width = image.Width;
            m_rect = new Rectangle(x, y, m_width, m_height);
            m_screenLocation = new Vector2(x, y);
        }

        public Rectangle LedgeRect
        {
            get { return m_rect; }
        }
    }
}
