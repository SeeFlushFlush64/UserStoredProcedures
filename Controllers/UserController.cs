
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PalaganasTechnicalExam.Models.Entities;
using PalaganasTechnicalExam.Models.ViewModels;
using PalaganasTechnicalExam.Repositories.Users;
using PalaganasTechnicalExam.Validators;
using System;

namespace PalaganasTechnicalExam.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<AddUserViewModel> _addUserValidator;
        public UserController(IUserRepository userRepository, IValidator<AddUserViewModel> addUserValidator)
        {
            _userRepository = userRepository;
            _addUserValidator = addUserValidator;
        }

   

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                users = await _userRepository.SearchUsersAsync(searchQuery);
            }

            return View(users);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel viewModel)
        {
            var result = await _addUserValidator.ValidateAsync(viewModel);

            if (!result.IsValid)
            {
                
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Validation Error: {error.PropertyName} - {error.ErrorMessage}");
                }

                result.AddToModelState(this.ModelState);
                return View("Add", viewModel);
            }

            var user = new User
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email
            };

            await _userRepository.AddUserAsync(user);
            TempData["AddedSuccessfully"] = "User added successfully";
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            var user = await _userRepository.GetUserByIdAsync(viewModel.UserId);
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Email = viewModel.Email;
            await _userRepository.UpdateUserAsync(user);
            TempData["UpdatedSuccessfully"] = "User updated successfully";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("List");
            }
            await _userRepository.DeleteUserAsync(user.UserId);
            TempData["DeletedSuccessfully"] = "User deleted successfully";
            return RedirectToAction("List");
        }


    }
}
