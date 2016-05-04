using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RallyProtractorIntegration
{

    public interface IEmail
    {
      void  SendEmail();
    }
    public class EmailProcessor : IEmail
    {
        public  void SendEmail()
        {
            throw new NotImplementedException("This method yet to Implememt");
        }
    }

    public abstract class EmailWorkItem : WorkItem<email>
    {
        static EmailWorkItem()
        {

        }
        public abstract override Task ExecuteWorkItem();
    }
}
