
using Microsoft.Extensions.Logging;
using Moq;
using PlayGoodService.Controllers;
using PlayGoodService.Security;

namespace PlayGoodService.Test
{
    [TestFixture]
    internal class AuthControllerTests
    {
        private AuthController _controller;
        private Mock<JwtSettings> _mockJwtSettings;
        private ILogger<AuthController> _logger;
    }

    
}
