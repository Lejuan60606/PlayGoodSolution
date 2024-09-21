
using Microsoft.Extensions.Logging;
using Moq;
using PlayGoodAssetService.Controllers;
using PlayGoodAssetService.Security;

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
