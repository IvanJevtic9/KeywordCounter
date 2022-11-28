namespace KeyWordCounterApp.Models
{
    public record struct Result(int One, int Two, int Three)
    {
        public static Result operator +(Result first, Result second) =>
            new Result(first.One + second.One, first.Two + second.Two, first.Three + second.Three);

        public override string ToString()
        {
            return $"[one={One}, two={Two}, three={Three}]";
        }
    }
}
