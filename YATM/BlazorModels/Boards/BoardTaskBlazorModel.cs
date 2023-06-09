using NetTopologySuite.Geometries;

namespace YATM.BlazorModels.Boards
{
    public class BoardTaskBlazorModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Geometry? MapGeometry { get; set; }
    }
}
