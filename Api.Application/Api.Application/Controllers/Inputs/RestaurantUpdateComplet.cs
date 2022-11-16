namespace Api.Application.Controllers.Inputs
{
    public class RestaurantUpdateComplet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Kitchen { get; set; }
        public string Publicplace { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
