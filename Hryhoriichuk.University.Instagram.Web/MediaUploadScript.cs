using Azure.Storage.Blobs;
using Hryhoriichuk.University.Instagram.Web.Controllers;
using Hryhoriichuk.University.Instagram.Web.Data;
using Hryhoriichuk.University.Instagram.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hryhoriichuk.University.Instagram.Web
{
    public class MediaUploadScript
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AuthDbContext _context;
        private readonly ILogger<MediaUploadScript> _logger;
        private readonly Random _random;

        public MediaUploadScript(BlobServiceClient blobServiceClient, AuthDbContext context, ILogger<MediaUploadScript> logger)
        {
            _blobServiceClient = blobServiceClient;
            _context = context;
            _logger = logger;
            _random = new Random();
        }

        public async Task UploadMediaForUsers()
        {
            var userIds = await _context.Users.Select(u => u.Id).ToListAsync();
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("instbydencontainer");
            var blobs = blobContainerClient.GetBlobsAsync();

            var blobUrls = new List<string>();
            await foreach (var blob in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(blob.Name);
                blobUrls.Add(blobClient.Uri.ToString());
            }

            if (!blobUrls.Any())
            {
                _logger.LogWarning("No images found in the Blob storage container.");
                return;
            }

            foreach (var userId in userIds)
            {
                for (int i = 0; i < 10; i++) // Create 10 posts per user
                {
                    var randomImage = blobUrls[_random.Next(blobUrls.Count)];
                    await CreatePost(userId, randomImage);
                }
            }
        }

        private async Task CreatePost(string userId, string imageUrl)
        {
            try
            {
                var post = new Post
                {
                    UserId = userId,
                    ImagePath = imageUrl,
                    DatePosted = DateTime.Now,
                    Caption = "Auto-uploaded image"
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Post created for user {userId} with image {imageUrl}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating a post for user {userId} with image {imageUrl}");
            }
        }
    }
}
