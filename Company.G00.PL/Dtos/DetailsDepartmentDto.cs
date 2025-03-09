using System.ComponentModel;

namespace Company.G00.PL.Dtos
{
    public class DetailsDepartmentDto
    {
        [ReadOnly(true)]
        public string Code { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }
        
        [ReadOnly(true)]
        public DateTime CreateAt { get; set; }
    }
}
