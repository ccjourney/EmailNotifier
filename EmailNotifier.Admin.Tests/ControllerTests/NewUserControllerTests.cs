using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmailNotifier.Admin.Controllers;
using EmailNotifier.Data;
using EmailNotifier.Events;
using EmailNotifier.Models;
using EmailNotifier.Publisher;
using Moq;
using NUnit.Framework;

namespace EmailNotifier.Admin.Tests.ControllerTests
{
    [TestFixture]
    public class NewUserControllerTests
    {
        private NewUserController _controller;
        private readonly Mock<INewUserRepository> _newUserRepository = new Mock<INewUserRepository>();
        private readonly Mock<IEmailPublisher> _emailPublisher = new Mock<IEmailPublisher>();
        private string _userEmail;
        private string _userId;
        private NewUser _newUser;

        [SetUp]
        public void SetUp()
        {
            _controller = new NewUserController(_newUserRepository.Object, _emailPublisher.Object);
            _userEmail = "test@test.com";
            _userId = Guid.NewGuid().ToString();
            _newUser = new NewUser { Name = "Test User", Email = _userEmail, Id = _userId, SentMail = false };
        }

        [Test]
        public async Task Should_Retreive_All_Users()
        {
            var allUsers = new List<NewUser> { new NewUser { Email = "test@test.com" } };
            _newUserRepository.Setup(x => x.GetAllUsers()).ReturnsAsync(allUsers);

            var result = await _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(allUsers));
            _newUserRepository.Verify(x => x.GetAllUsers(), Times.Once);
        }

        [Test]
        public async Task Should_Email_Selected_User()
        {
            _newUserRepository.Setup(x => x.GetNewUser(_userId)).ReturnsAsync(_newUser);

            await _controller.Email(_userEmail, _userId);

            _emailPublisher.Verify(x => x.SendEmail(_userEmail), Times.Once);
        }

        [Test]
        public async Task Should_Load_Details_Of_Selected_User_To_Email()
        {
            _newUserRepository.Setup(x => x.GetNewUser(_userId)).ReturnsAsync(_newUser);

            await _controller.Email(_userEmail, _userId);

            _newUserRepository.Verify(x => x.GetNewUser(_userId), Times.Once);
        }

        [Test]
        public async Task Should_Update_SentEmail_Flag_Of_Selected_User_To_Email()
        {
            _newUserRepository.Setup(x => x.GetNewUser(_userId)).ReturnsAsync(_newUser);

            await _controller.Email(_userEmail, _userId);

            _newUserRepository.Verify(x => x.UpdateNewUser(_userId, 
                It.Is<NewUser>( u => u.Email.Equals(_newUser.Email) 
                && u.Id.Equals(_newUser.Id) 
                && u.SentMail)), Times.Once);
        }

        [Test]
        public async Task Should_Refresh_User_List_After_Emailing_A_Selected_User()
        {
            _newUserRepository.Setup(x => x.GetNewUser(_userId)).ReturnsAsync(_newUser);

            var result = await _controller.Email(_userEmail, _userId) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public async Task Should_Add_New_User()
        {
            await _controller.AddNewUser(_newUser);

            _newUserRepository.Verify(x => x.CreateNewUser(_newUser), Times.Once);
        }

        [Test]
        public async Task Should_Flag_If_Name_Is_Not_Supplied_When_adding_New_User()
        {
            _newUser.Name = string.Empty;

            var result = await _controller.AddNewUser(_newUser) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewData.ModelState.Values.First().Errors.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Should_Flag_If_Email_Is_Not_Supplied_When_adding_New_User()
        {
            _newUser.Email = string.Empty;

            var result = await _controller.AddNewUser(_newUser) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewData.ModelState.Values.First().Errors.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Should_Flag_If_Both_Email_And_Name_Are_Not_Supplied_When_adding_New_User()
        {
            _newUser.Email = string.Empty;
            _newUser.Name = string.Empty;

            var result = await _controller.AddNewUser(_newUser) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewData.ModelState.Values.First().Errors.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Should_Refresh_User_List_After_Adding_A_New_User()
        {
            var result = await _controller.AddNewUser(_newUser) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }
    }
}
