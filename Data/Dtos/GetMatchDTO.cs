namespace api.Data.Dtos
{
    public class GetMatchDTO
    {
        public int Id { get; set; }
        public SimplPlayerDTO Winner { get; set; }
        public SimplPlayerDTO Loser { get; set; }
    }
}
