using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using UvarTo.Controllers;
using UvarTo.Domain.Entities;
using UvarTo.Application.Abstraction;
using UvarTo.Application.Implementation;

namespace UvarTo.Test
{
    public class AddTipTest
    {
        [Fact]
        public async Task AddTip_ValidTip_RedirectsToIndex()
        {
            var tipsServiceMock = new Mock<ITipsService>();
            tipsServiceMock.Setup(x => x.GetCurrentId()).Returns("testUserId");
            tipsServiceMock.Setup(x => x.AddTip(It.IsAny<Tips>())).Verifiable();

            var searchTServiceMock = new Mock<ISearchTService>();
            var controller = new TipsController(searchTServiceMock.Object, tipsServiceMock.Object);

            var tipsToAdd = new Tips { TipName = "TestTip", TipText = "TestText" };

            var result = await controller.Create(tipsToAdd) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            tipsServiceMock.Verify(x => x.AddTip(It.IsAny<Tips>()), Times.Once);
        }
    }
}