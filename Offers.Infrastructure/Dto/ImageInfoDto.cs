using System;

namespace Offers.Infrastructure.Dto
{
    public class ImageInfoDto
    {
        public Guid Id { get; init; }
        public string Uri { get; init; }
        public int? SortId { get; init; }
        public bool IsMain { get; init; }
    }
}