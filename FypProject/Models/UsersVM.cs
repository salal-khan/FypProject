using FypProject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class UsersVM
    {
    }
    public class CreateUserVM
    {


        [Required, Display(Name = "Email", Description = "User email for contacting"), EmailAddress]
        [System.Web.Mvc.Remote("DoesEmailExist", "User", HttpMethod = "POST", ErrorMessage = "Email already exists. Please enter a different email.")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "UserName")]
        [System.Web.Mvc.Remote("DoesUserNameExist", "User", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CNIC { get; set; }



        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        private SelectList _RoleLIst { get; set; }
        public SelectList RoleList
        {
            get
            {

                if (_RoleLIst != null) return _RoleLIst;
                return new SelectList(GlobalHelpers.GetRoleList(), "RoleId", "RoleName", this.RoleId);

            }
            set { _RoleLIst = value; }
        }



        [Required]
        [Display(Name = "Franchise")]
        public int FranchiseId { get; set; }
        private SelectList _FranchiseLIst { get; set; }
        public SelectList FranchiseList
        {
            get
            {

                if (_FranchiseLIst != null) return _FranchiseLIst;
                return new SelectList(GlobalHelpers.GetFranchiseList(), "FranchiseId", "FranchiseName", this.FranchiseId);

            }
            set { _FranchiseLIst = value; }
        }




    }
    public class IndexUserVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleName { get; set; }
        public string FranchiseName { get; set; }
    }
    public class DetailUserVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleName { get; set; }
        public string FranchiseName { get; set; }
        public string Address { get; set; }
        public string Cnic { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class EditUserVM
    {

        public int Id { get; set; }
        [Required, Display(Name = "Email", Description = "User email for contacting"), EmailAddress]

        public string Email { get; set; }


        [Required]
        [Display(Name = "UserName")]

        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

       
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CNIC { get; set; }
       



        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        private SelectList _RoleLIst { get; set; }
        public SelectList RoleList
        {
            get
            {

                if (_RoleLIst != null) return _RoleLIst;
                return new SelectList(GlobalHelpers.GetRoleList(), "RoleId", "RoleName", this.RoleId);

            }
            set { _RoleLIst = value; }
        }



        [Required]
        [Display(Name = "Franchise")]
        public int? FranchiseId { get; set; }
        private SelectList _FranchiseLIst { get; set; }
        public SelectList FranchiseList
        {
            get
            {

                if (_FranchiseLIst != null) return _FranchiseLIst;
                return new SelectList(GlobalHelpers.GetFranchiseList(), "FranchiseId", "FranchiseName", this.FranchiseId);

            }
            set { _FranchiseLIst = value; }
        }




    }

}