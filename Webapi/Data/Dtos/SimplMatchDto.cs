using System;

namespace Webapi.Data.Dtos
{
    public class SimplMatchDto
    {
        public int Id { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }

        public List<SimplPlayerDTO> Players { get; set; }
    }
}
