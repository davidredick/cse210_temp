namespace Journal2_0.Models
{
    public class Entry
    {
        public string Date { get; set; }
        public string Prompt { get; set; }
        public string Response { get; set; }

        public Entry() { }

        public Entry(string date, string prompt, string response)
        {
            Date = date;
            Prompt = prompt;
            Response = response;
        }

        public override string ToString()
        {
            return $"[{Date}] {Prompt}\n{Response}";
        }
    }
}