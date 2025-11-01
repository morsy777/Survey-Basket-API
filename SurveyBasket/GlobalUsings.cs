global using System.Text;
global using System.Reflection;
global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;
global using System.ComponentModel.DataAnnotations;
global using System.Security.Cryptography;

global using Mapster;
global using MapsterMapper;

global using FluentValidation;
global using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Cors;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.Extensions.Options;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.IdentityModel.Tokens;

global using SurveyBasket;
global using SurveyBasket.Abstractions;
global using SurveyBasket.Authentication;
global using SurveyBasket.Abstractions.Consts;
global using SurveyBasket.Contracts.Authentication;
global using SurveyBasket.Contracts.Polls;
global using SurveyBasket.Contracts.User;
global using SurveyBasket.Entities;
global using SurveyBasket.Errors;
global using SurveyBasket.Extensions;
global using SurveyBasket.Services;
global using SurveyBasket.Settings;
global using SurveyBasket.Helpers;
global using SurveyBasket.Persistence; 

global using MimeKit;
global using MailKit;
global using MailKit.Net.Smtp;
global using MailKit.Security;

global using Hangfire;
global using HangfireBasicAuthenticationFilter;



