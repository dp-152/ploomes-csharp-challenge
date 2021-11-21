﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Util
{
  public class UserValidator
  {
    private readonly IUserRepository _userRepository;

    public UserValidator(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public void ValidateUserToken(out User? user, IHeaderDictionary headers, ModelStateDictionary modelState)
    {
      headers.TryGetValue("Authorization", out var token);
      if (token.Count < 1 || string.IsNullOrWhiteSpace(token[0]))
      {
        user = null;
        modelState.AddModelError("AuthToken", "Invalid token or user does not exist");
        return;
      }

      user = _userRepository.GetByToken(token[0]);
    }

    public void ValidateUserExists(out User? user, int userId, ModelStateDictionary modelState)
    {
      user = _userRepository.GetById(userId);
      if (user is null)
      {
        modelState.AddModelError("User", $"No such user with ID #{userId}");
      }
    }
  }
}