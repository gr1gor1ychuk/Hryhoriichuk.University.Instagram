using FakeItEasy;
using FluentAssertions;
using Hryhoriichuk.University.Instagram.Web.Areas.Identity.Data;
using Hryhoriichuk.University.Instagram.Web.Controllers;
using Hryhoriichuk.University.Instagram.Web.Data;
using Hryhoriichuk.University.Instagram.Web.Models;
using Hryhoriichuk.University.Instagram.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hryhoriichuk.University.Instagram.Test.Controller
{
    public class PostControllerTests
    {
        private PostController _postController;
        private UserManager<ApplicationUser> _userManager;
        private AuthDbContext _context;
        private INotificationService _notificationService;
        private IWebHostEnvironment _webHostEnvironment;

        public PostControllerTests()
        {
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _context = A.Fake<AuthDbContext>();
            _notificationService = A.Fake<INotificationService>();
            _webHostEnvironment = A.Fake<IWebHostEnvironment>();

        }

        [Fact]
        public void PostController_AddComment_ReturnsSuccess()
        {
            var postId = 1;
            var post = A.Fake<Post>();
            A.CallTo(() => _context.Posts.FindAsync(postId)).Returns(post);
            var commentText = "ogo";

            var result = _postController.AddComment(postId, commentText);

            result.Should().BeOfType < Task<IActionResult>>();
        }
    }
}
