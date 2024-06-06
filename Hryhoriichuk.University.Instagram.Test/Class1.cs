using Azure.Storage.Blobs;
using Hryhoriichuk.University.Instagram.Web.Areas.Identity.Data;
using Hryhoriichuk.University.Instagram.Web.Controllers;
using Hryhoriichuk.University.Instagram.Web.Data;
using Hryhoriichuk.University.Instagram.Web.Models;
using Hryhoriichuk.University.Instagram.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hryhoriichuk.University.Instagram.Test
{
    [TestClass]
    public class PostControllerTests
    {
        private AuthDbContext _context;
        private PostController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            _context = new AuthDbContext(options);
            SeedData();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                FullName = "Test User",
                ProfilePicturePath = "path/to/profile/picture.jpg"
            });

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
                }))
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        private void SeedData()
        {
            var userId = "test-user-id";
            var postId = 1;

            // Seed user
            _context.Users.Add(new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                FullName = "Test User",
                ProfilePicturePath = "path/to/profile/picture.jpg"
            });

            // Seed post
            _context.Posts.Add(new Post
            {
                Id = postId,
                ImagePath = "path/to/image.jpg",
                UserId = userId, // Ensure the post belongs to the test user
                                 // Other properties
            });

            // Seed like
            _context.Likes.Add(new Like
            {
                PostId = postId,
                UserId = userId
            });

            _context.SaveChanges();
        }


        [TestMethod]
        public async Task ToggleLike_AddsLike_WhenNotLiked()
        {
            // Arrange
            var postId = 1;
            var userId = "test-user-id";

            // Act
            var result = await _controller.ToggleLike(postId);

            // Assert
            var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            Assert.IsNotNull(like);
        }

    }
}
