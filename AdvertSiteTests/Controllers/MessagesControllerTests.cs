using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class MessagesControllerTests : IDisposable
    {
        private ApplicationUser fakeUser;
        private MockRepository mockRepository;
        private advert_siteContext mockadvert_siteContext;
        private UserManager<ApplicationUser> mockUserManager;

        public MessagesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockadvert_siteContext = TestHelpers.CreateFakeDbContext();
            this.mockUserManager = TestHelpers.TestUserManager<ApplicationUser>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
            mockadvert_siteContext.Database.EnsureDeleted();
        }

        private MessagesController CreateMessagesController(bool withUser = false)
        {

            var contr = new MessagesController(this.mockadvert_siteContext, this.mockUserManager);
            if (withUser)
            {
                (this.fakeUser, contr.ControllerContext) = TestHelpers.FakeUserAndControllerContext(this.mockadvert_siteContext);
            }

            return contr;
        }

        [Fact]
        public void Index_Messages_ShouldReturnRedirectToIndox()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            // Act
            var result = messagesController.Index();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(50)]
        public async Task Inbox_Messages_ReturnUserMessagesView(int msgCount)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            //create sender
            var user = new ApplicationUser() {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            //create messages
            for (int i = 0; i < msgCount; i++)
            {
                var msg = new Messages()
                {
                    DateSent = DateTime.Now,
                    Subject = "TestMsg" + i,
                    Text = "yo" + i
                };
                mockadvert_siteContext.Messages.Add(msg);
                mockadvert_siteContext.SaveChanges();

                var userHasMsg = new UsersHasMessages()
                {
                    MessagesId = msg.Id,
                    RecipientId = fakeUser.Id,
                    SenderId = user.Id,
                    IsDeleted = 0
                };


                mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
                mockadvert_siteContext.SaveChanges(); 
            }

            // Act
            var result = await messagesController.Inbox();
            var viewResult = (ViewResult)result;
            var messages = (IEnumerable<UsersHasMessages>)viewResult.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(msgCount, messages.Count());
        }

        [Theory]
        [InlineData(20)]
        [InlineData(10)]
        public async Task Outbox_Messages_ReturnUserSentMessageView(int msgCount)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            //recipient sender
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            //create messages
            for (int i = 0; i < msgCount; i++)
            {
                var msg = new Messages()
                {
                    DateSent = DateTime.Now,
                    Subject = "TestMsg" + i,
                    Text = "yo" + i
                };
                mockadvert_siteContext.Messages.Add(msg);
                mockadvert_siteContext.SaveChanges();

                var userHasMsg = new UsersHasMessages()
                {
                    MessagesId = msg.Id,
                    RecipientId = user.Id,
                    SenderId = fakeUser.Id,
                    IsDeleted = 0,
                    IsAdminMessage = 0
                };

                mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
                mockadvert_siteContext.SaveChanges();
            }

            // Act
            var result = await messagesController.Outbox();
            var viewResult = (ViewResult)result;
            var messages = (IEnumerable<UsersHasMessages>)viewResult.Model;
            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(msgCount, messages.Count());
        }

        [Fact]
        public async Task Details_MessageId_OpenSelectedMessageView()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            //create sender
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            //add msg
            var msgToAdd = new Messages()
            {
                DateSent = DateTime.Now,
                Subject = "TefstMsg",
                Text = "yoeqragh"
            };
            mockadvert_siteContext.Messages.Add(msgToAdd);
            mockadvert_siteContext.SaveChanges();

            var userHasMsg = new UsersHasMessages()
            {
                MessagesId = msgToAdd.Id,
                RecipientId = fakeUser.Id,
                SenderId = user.Id,
                IsDeleted = 0
            };
            mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
            mockadvert_siteContext.SaveChanges();

            // Act
            var result = await messagesController.Details(msgToAdd.Id);
            var viewResult = (ViewResult)result;
            var addedMsg = (Messages)viewResult.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(msgToAdd.Id, addedMsg.Id);
            Assert.Equal(msgToAdd.Text, addedMsg.Text);
            Assert.Equal(msgToAdd.Subject, addedMsg.Subject);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(null)]
        public async Task Details_InvalidId_OpenNotFoundView(int id)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            // Act
            var result = await messagesController.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void CreateAdmin_UserIsAdmin_OpenCreateAdminMessageView()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Query["recipientId"]).Returns(this.fakeUser.Id);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };
            messagesController.ControllerContext = controllerContext;
            // Act


            var result = messagesController.CreateAdmin();
            var resultView = (ViewResult)result;
            var sender = (CreateMessageModel)resultView.Model;


            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<CreateMessageModel>(resultView.Model);
            Assert.Equal(this.fakeUser.Id, sender.RecipientId);
        }

        [Fact]
        public void Create_RecipientAndSubject_OpenCreateMessageView()
        {

            // Arrange

            var messagesController = this.CreateMessagesController(true);
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            var identity = new GenericIdentity(fakeUser.Id, ClaimTypes.NameIdentifier);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, fakeUser.Id));
            var fakeUserIdent = new GenericPrincipal(identity, new string[] { "User" });


            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Query["recipientId"]).Returns(user.Id);
            httpContext.Setup(x => x.Request.Query["subject"]).Returns(this.fakeUser.Id);
            httpContext.Setup(x => x.User).Returns(fakeUserIdent);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };
            messagesController.ControllerContext = controllerContext;


            // Act


            var result = messagesController.Create();
            var resultView = (ViewResult)result;
            var sender = (CreateMessageModel)resultView.Model;


            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<CreateMessageModel>(resultView.Model);
            Assert.Equal(user.Id, sender.RecipientId);
        }

        [Fact]
        public void Create_UserIsRecipient_RedirectToMainView()
        {
            // Arrange

            var messagesController = this.CreateMessagesController(true);

            var identity = new GenericIdentity(fakeUser.Id, ClaimTypes.NameIdentifier);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, fakeUser.Id));
            var fakeUserIdent = new GenericPrincipal(identity, new string[] { "User" });

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Query["recipientId"]).Returns(this.fakeUser.Id);
            httpContext.Setup(x => x.Request.Query["subject"]).Returns(this.fakeUser.Id);
            httpContext.Setup(x => x.User).Returns(fakeUserIdent);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };
            messagesController.ControllerContext = controllerContext;


            // Act
            var result = messagesController.Create();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task CreateAdmin_AdminMessage_SendAdminMessageToUsers()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            }; 
            var user2 = new ApplicationUser()
            {
                UserName = "Ben",
                Email = "Smithinges@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user2);
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();
            CreateMessageModel model = new CreateMessageModel()
            {
                RecipientId = this.fakeUser.Id,
                Message = new Messages()
                {
                    Subject = "test",
                    Text = "This is a test message"
                }
                
            };
            // Act
            var result = await messagesController.CreateAdmin(model);
            var messageCount = mockadvert_siteContext.UsersHasMessages.Where(x => x.IsAdminMessage == 1 && x.SenderId.Equals(this.fakeUser.Id)).Count();
            var userCount = mockadvert_siteContext.Users.Count();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(userCount, messageCount);
        }

        [Fact]
        public async Task Create_Message_SendMessage()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            var userRecipient = new ApplicationUser() {
                Email = "AAGGEE@gmail.com",
                UserName = "Bob"
            };
            mockadvert_siteContext.Users.Add(userRecipient);
            mockadvert_siteContext.SaveChanges();

            var msg = new Messages() {
                Subject = "Helo",
                Text = "Sup"
            };


            var userHasMsgs = new UsersHasMessages() {
                IsDeleted = 0,
                MessagesId = msg.Id,
                RecipientId = userRecipient.Id,
                Sender = fakeUser,
                Messages = msg
            };
            var model = new CreateMessageModel() {
                Message = msg,
                RecipientId = userRecipient.Id,
                UsersHasMessages = userHasMsgs
            };

            // Act
            var result = await messagesController.Create(model);
            var redirResult = (RedirectToActionResult)result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(messagesController.Index), redirResult.ActionName);
        }

        [Fact]
        public async Task MarkAsRead_Message_MarkMessageAsRead()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);

            var msg = new UsersHasMessages()
            {
                Messages = new Messages()
                {
                    Subject = "Test Subject",
                    Text = "Test Text"
                },
                RecipientId = this.fakeUser.Id,
                SenderId = user.Id,
                IsAdminMessage = 0,
                AlreadyRead = 0,
                IsDeleted = 0
            };
            mockadvert_siteContext.UsersHasMessages.Add(msg);

            mockadvert_siteContext.SaveChanges();

            // Act
            var result = await messagesController.MarkAsRead(msg.MessagesId, user.Id, fakeUser.Id);

            var userMessage = mockadvert_siteContext.UsersHasMessages.FirstOrDefault(x => x.MessagesId == msg.MessagesId && user.Id.Equals(x.SenderId) && fakeUser.Id.Equals(x.RecipientId));

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(userMessage.AlreadyRead == 1);
        }

        [Fact]
        public async Task Delete_Message_DeleteMessage()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            //add recipient
            var recipientUser = new ApplicationUser() {
                Email = "lglaerg@gmail.com",
                UserName = "Jerry"
            };
            mockadvert_siteContext.Add(recipientUser);

            //add msg
            var msg = new Messages() {
                Subject = "Delete me",
                Text = "LOL"
            };
            mockadvert_siteContext.Add(msg);
            mockadvert_siteContext.SaveChanges();

            var userHasMsg = new UsersHasMessages() {
                MessagesId = msg.Id,
                RecipientId = recipientUser.Id,
                SenderId = fakeUser.Id,
                IsDeleted = 0
            };
            mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
            mockadvert_siteContext.SaveChanges();


            // Act
            var result = await messagesController.Delete(
                userHasMsg.MessagesId,
                userHasMsg.SenderId,
                userHasMsg.RecipientId);

            // Assert
            Assert.Equal((short)1, userHasMsg.IsDeleted);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(10, 8)]
        [InlineData(10, 2)]
        public void UpdateUnreadMessageCount_Messages_ReturnUnreadMessageCount(int messageCount, int unreadCount)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            //recipient sender
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            int counter = 0;
            //create messages
            for (int i = 0; i < messageCount; i++)
            {
                var msg = new Messages()
                {
                    DateSent = DateTime.Now,
                    Subject = "TestMsg" + i,
                    Text = "yo" + i
                };
                mockadvert_siteContext.Messages.Add(msg);
                mockadvert_siteContext.SaveChanges();

                var userHasMsg = new UsersHasMessages()
                {
                    MessagesId = msg.Id,
                    RecipientId = fakeUser.Id,
                    SenderId = user.Id,
                    IsDeleted = 0,
                    IsAdminMessage = 0,
                    AlreadyRead = 1
                };

                if (counter < unreadCount)
                {
                    userHasMsg.AlreadyRead = 0;
                    counter++;
                }

                mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
                mockadvert_siteContext.SaveChanges();
            }

            // Act
            var result = messagesController.UpdateUnreadMessageCount();

            // Assert
            Assert.Equal(unreadCount, result);
        }

        [Fact]
        public void GetRecipientUser_RecipientId_ReturnRecipientObject()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            // Act
            var result = messagesController.GetRecipientUser(this.fakeUser.Id);

            // Assert
            Assert.Equal(this.fakeUser.Id, result.Id);
        }
    }
}
