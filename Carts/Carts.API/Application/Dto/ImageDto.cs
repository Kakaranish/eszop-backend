using System;

namespace Carts.API.Application.Dto
{
    public class ImageDto
    {
        public Guid Id { get; init; }
        public string Uri { get; init; }
        public bool IsMain { get; init; }
    }
}
