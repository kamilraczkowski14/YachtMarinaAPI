using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YachtMarinaAPI.Controllers;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;
using Assert = Xunit.Assert;

namespace YachtMarinaAPI.Tests.Controllers
{
    public class JourneyControllerTests
    {
       
        [Fact]
        public async Task GetAll_ReturnsOkObjectResultWithListOfJourneyDto()
        {
            var journeyDtos = new List<JourneyDto>();
            var serviceMock = new Mock<IJourneyService>();
            serviceMock.Setup(x => x.GetAll()).ReturnsAsync(journeyDtos);

            var controller = new JourneyController(serviceMock.Object);
            var result = await controller.GetAll();

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        }

    }
}





