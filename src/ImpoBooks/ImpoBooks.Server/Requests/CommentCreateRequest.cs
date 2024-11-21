using System.ComponentModel.DataAnnotations;

namespace ImpoBooks.Server.Requests
{
	public class CommentCreateRequest
	{
		[Required] public int UserId { get; set; }
		[Required] public string Content { get; set; }
		[Required] public decimal Rating { get; set; }
	}
}
