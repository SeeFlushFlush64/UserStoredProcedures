
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
        private readonly IValidator<EditUserViewModel> _editUserValidator;
        private const int PageSize = 12;
        public UserController(IUserRepository userRepository, IValidator<AddUserViewModel> addUserValidator, IValidator<EditUserViewModel> editUserValidator)
        {
            _userRepository = userRepository;
            _addUserValidator = addUserValidator;
            _editUserValidator = editUserValidator;
        }



        //[HttpGet]
        //public async Task<IActionResult> List(string? searchQuery, int pageNumber = 1)
        //{
        //    var users = await _userRepository.GetAllUsersAsync();

        //    if (!string.IsNullOrEmpty(searchQuery))
        //    {
        //        users = await _userRepository.SearchUsersAsync(searchQuery);
        //    }

        //    int totalUsers = users.Count();
        //    var pagedUsers = users.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

        //    var viewModel = new PaginatedUserListViewModel
        //    {
        //        Users = pagedUsers,
        //        PageNumber = pageNumber,
        //        TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize),
        //        SearchQuery = searchQuery
        //    };

        //    return View(viewModel);
        //}
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, string sortColumn = "FirstName", string sortOrder = "ASC", int pageNumber = 1)
        {
            var users = await _userRepository.GetSortedUsersAsync(searchQuery, sortColumn, sortOrder, pageNumber, PageSize);
            var userList = users.ToList();
         

            int totalUsers = _userRepository.GetTotalUserCount();

            var viewModel = new PaginatedUserListViewModel
            {
                Users = users,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize),
                SearchQuery = searchQuery,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };

            return View(viewModel);
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
                Email = viewModel.Email,
                ProfilePictureUrl = viewModel.ProfilePictureUrl
            };

            await _userRepository.AddUserAsync(user);
            TempData["AddedSuccessfully"] = "User added successfully";
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {   
            var user = await _userRepository.GetUserByIdAsync(id);
            var userViewModel = new EditUserViewModel
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            var user = await _userRepository.GetUserByIdAsync(viewModel.UserId);
            var result = await _editUserValidator.ValidateAsync(viewModel);
            
            if (!result.IsValid)
            {

                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Validation Error: {error.PropertyName} - {error.ErrorMessage}");
                }

                result.AddToModelState(this.ModelState);
                return View("Edit", viewModel);
            }
            // Check if any changes were made
            if (user.FirstName == viewModel.FirstName &&
                user.LastName == viewModel.LastName &&
                user.Email == viewModel.Email &&
                user.ProfilePictureUrl == viewModel.ProfilePictureUrl)
            {
                return RedirectToAction("List"); // No update needed, just redirect
            }
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Email = viewModel.Email;
            user.ProfilePictureUrl = viewModel.ProfilePictureUrl;
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
