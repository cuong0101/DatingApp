using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likeRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likeRepository)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> Addlike(string username){
            var sourceUserId = User.GetUserId();
            var likeUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);
            
            if(likeUser == null) return NotFound();
            
            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likeRepository.GetUserLike(sourceUserId, likeUser.Id);

            if(userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike {
                SourceUserId = sourceUserId,
                LikedUserId = likeUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }  

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUerLikers([FromQuery]LikesParams likesParams){
            likesParams.UserId = User.GetUserId();
            var users = await _likeRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}