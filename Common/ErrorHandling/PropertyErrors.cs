using System.Collections.Generic;

namespace Common.Utilities.ErrorHandling
{
    public class PropertyErrors
    {
        public string Property { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}
