namespace ZooAppUsingSp.ReceiptNoGenaration
{
    public class ReceiptNo
    {
        public static string Get()
        {
            return DateTime.Now.ToString("yyMMddhhmmssffff");
        }
    }
}
