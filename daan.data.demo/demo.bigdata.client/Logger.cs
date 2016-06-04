using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace demo.bigdata.client
{
    public class Logger
    {
        private TextBox _textbox;
        private StringBuilder sb = new StringBuilder();

        public Logger(TextBox tbx)
        {
            _textbox = tbx;
        }
              

        public void Log(string message)
        {
            string formattedMessage = string.Format("[{0}]: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
            sb.AppendLine(formattedMessage);
            _textbox.Text = sb.ToString();
        }
    }
}
