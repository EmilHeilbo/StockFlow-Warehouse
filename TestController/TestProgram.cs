using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestWarehouse
{
    public class TestProgram
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task GetProducts_Returns200()
        {
            var response = await _client.GetAsync("/products");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetProducts_ReturnsEmptyList()
        {
            var response = await _client.GetAsync("/products");
            var body = await response.Content.ReadAsStringAsync();

            Assert.That(body, Is.EqualTo("[]"));
        }
    }
}
