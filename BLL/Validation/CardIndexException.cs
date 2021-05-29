using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validation
{
    public class CardIndexException : Exception
    {
        private string message;

        public string GetMessage()
        {
            return message;
        }

        public void SetMessage(string value)
        {
            message = value;
        }

        public CardIndexException() { }

        public CardIndexException(string message):base(message)
        {
            SetMessage(message);
        }
    }
}
