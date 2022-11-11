namespace Api.Application.Controllers.Outputs
{
    public class RestaurantView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Kitchen { get; set; }
        public AddressView Address { get; set; }

    }
}
