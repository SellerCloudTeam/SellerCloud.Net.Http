using System;

namespace SellerCloud.Net.Http.Models
{
    public class FileAttachment
    {
        public FileAttachment(string name, byte[] content, string contentType)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Content = content ?? throw new ArgumentNullException(nameof(content));
            this.ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        public string Name { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
    }
}
