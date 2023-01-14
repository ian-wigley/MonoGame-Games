using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPac
{
    public class Explosion : BaseObject
    {
        private bool m_animationComplete = false;

        public Explosion(int x, int y, Texture2D image)
        {
            m_image = image;
            m_height = image.Height;
            m_width = image.Width / 16;
            m_screenLocation = new Vector2(x, y);
        }

        public void Update()
        {
            m_rect = new Rectangle(m_frame * m_width, 0, m_width, m_height);
            m_delayCounter += m_elapsedCounter;
            if (m_delayCounter > 0.2)
            {
                m_delayCounter = 0;
                if (m_frame < 16)
                {
                    m_frame += 1;
                }
                else
                {
                    m_animationComplete = true;
                }
            }
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_image, m_screenLocation, m_rect, Color.White, 0, Vector2.Zero, 1.0f, 0, 0);
        }

        public bool AnimationComplete { get { return m_animationComplete; } }

    }
}
