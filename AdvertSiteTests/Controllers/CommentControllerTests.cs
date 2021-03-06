using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class CommentControllerTests : IDisposable
    {
        private ApplicationUser fakeUser;

        private MockRepository mockRepository;

        private advert_siteContext mockadvert_siteContext;
        private UserManager<ApplicationUser> mockUserManager;

        public CommentControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockadvert_siteContext = TestHelpers.CreateFakeDbContext();
            this.mockUserManager = TestHelpers.TestUserManager<ApplicationUser>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
            mockadvert_siteContext.Database.EnsureDeleted();
        }

        private CommentController CreateCommentController(bool withUser)
        {
            var commentController =  new CommentController(this.mockadvert_siteContext, this.mockUserManager);
            if (withUser)
            {
                (fakeUser, commentController.ControllerContext) = TestHelpers.FakeUserAndControllerContext(mockadvert_siteContext);
            }

            return commentController;
        }

        [Fact]
        public async Task Create_ListingId_CommentOpenView()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);
            var listing = new Listings()
            {
                Name = "Cool train",
                Description = "The greatest train of all time",
                Userid = this.fakeUser.Id,
                Display = 1,
                Verified = 1,
                Price = 500000.0
            };
            mockadvert_siteContext.Listings.Add(listing);
            await mockadvert_siteContext.SaveChangesAsync();

            // Act
            var result = commentController.Create(listing.Id);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidListingId_RedirectToMainPage()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);

            // Act
            var result = commentController.Create(450);
            var resultView = (RedirectToActionResult)result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", resultView.ActionName);
            Assert.Equal("Home", resultView.ControllerName);
        }

        [Fact]
        public async Task CreateAsync_Comment_CreateNewComment()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);
            Comments comment = new Comments() {
                Text = "TestComment"
            };
            

            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "Test Nice Car",
                Description = "free car :)",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();
            //get id
            int id = listing.Id;


            // Act
            var result = (RedirectToActionResult)await commentController.CreateAsync(id, comment);
            var addedComment = await mockadvert_siteContext.Comments.FirstAsync(c => c.Userid == fakeUser.Id);

            // Assert
            Assert.Equal(addedComment.Text, comment.Text);
            Assert.Equal(addedComment.Userid, fakeUser.Id);
            
        }
        [Fact]
        public async Task CreateAsync_InvalidModelState_RedirectToMainPage()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);
            commentController.ModelState.AddModelError("key", "message");

            int id;

            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "good Car",
                Description = "easy free car :(",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();

            //setup data
            id = listing.Id;
            ListingAndComment listingAndComment = new ListingAndComment()
            {
                Comment = new Comments()
                {
                    Text = "GREAT CAR"
                },
                Listing = listing
            };

            var result = await commentController.CreateAjax(id, listingAndComment);
            var resultView = (RedirectToActionResult)result;

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", resultView.ActionName);
            Assert.Equal("Home", resultView.ControllerName);
        }
        [Fact]
        public async Task CreateAjax_Comment_ShouldCreateComment()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);
            int id;

            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "good Car",
                Description = "easy free car :(",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();

            //setup data
            id = listing.Id;
            ListingAndComment listingAndComment = new ListingAndComment() {
                Comment = new Comments()
                {
                    Text = "GREAT CAR"
                },
                Listing = listing
            };

            // Act
            var result = (OkResult)await commentController.CreateAjax(id, listingAndComment);
            var addedComment = await mockadvert_siteContext.Comments.FirstOrDefaultAsync(c => c.Id == listingAndComment.Comment.Id);


            // Assert
            Assert.NotNull(addedComment);
            Assert.Equal(listingAndComment.Comment.Text, addedComment.Text);
            Assert.Equal(fakeUser.Id, addedComment.Userid);
            Assert.Equal(200, result.StatusCode);
            
        }

        [Fact]
        public void Delete_CommentId_OpenViewResult ()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);

            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "good Car",
                Description = "easy free car :(",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();

            var comment = new Comments()
            {
                Listingid = listing.Id,
                Text = "HAHA",
            };
            mockadvert_siteContext.Comments.Add(comment);
            mockadvert_siteContext.SaveChanges();

            var result = commentController.Delete(comment.Id);

            // Assert
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public void Delete_InvalidId_OpenRedirectToMainPage()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);

            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "good Car",
                Description = "easy free car :(",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();

            var result = commentController.Delete(8648);
            var res = (RedirectToActionResult)result;
            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", res.ActionName);
            Assert.Equal("Home", res.ControllerName);
        }


        [Fact]
        public void Delete_UnspecifiedId_OpenNotFoundView()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);

            var result = commentController.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_Comment_DeleteSelectedComment()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);
            
            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "good Car",
                Description = "easy free car :(",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();

            Comments comment = new Comments() {
                Listingid = listing.Id,
                Text = "Nice car, how much",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Comments.Add(comment);
            mockadvert_siteContext.SaveChanges();


            // Act
            var result = await commentController.DeleteConfirmed(comment.Id);
            var deletedComment = mockadvert_siteContext.Comments.FirstOrDefault(c => c.Id == comment.Id);
            var listingCommentList = mockadvert_siteContext.Listings.Include(l => l.Comments).FirstOrDefault(l => l.Id == listing.Id).Comments;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(deletedComment);
            Assert.Equal(0, listingCommentList.Count);
        }

        [Fact]
        public async Task GetComments_ShouldGetAllListing_sComments()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);

            //create listing
            var listing = new Listings() {
                Name = "Free computer",
                Description = "I'm giving away free pc",
                Price = 0,
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();

            //add comments to listing
            List<Comments> commentList = new List<Comments>() {
                new Comments() { Listingid = listing.Id, Text = "Nice pc", Userid = fakeUser.Id},
                new Comments() { Listingid = listing.Id, Text = "Nice pc", Userid = fakeUser.Id},
                new Comments() { Listingid = listing.Id, Text = "WOW", Userid = fakeUser.Id}
            };
            foreach (var comment in commentList)
            {
                mockadvert_siteContext.Comments.Add(comment);
            }
            mockadvert_siteContext.SaveChanges();

            // Act
            var result = await commentController.GetComments(listing.Id);
            var okObjResult = (OkObjectResult)result;
            var comments = (IEnumerable<dynamic>)okObjResult.Value;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjResult.StatusCode);
            Assert.Equal(3, comments.Count());
        }

    }
}
