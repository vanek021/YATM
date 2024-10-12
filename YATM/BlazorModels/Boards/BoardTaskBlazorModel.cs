using NetTopologySuite.Geometries;
using YATM.Models.Entities;

namespace YATM.BlazorModels.Boards
{
    public class BoardTaskBlazorModel
    {
        public long Id { get; set; }
        public long BoardColumnId { get; set; }
        public User? User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Geometry? MapGeometry { get; set; }
    }
}
