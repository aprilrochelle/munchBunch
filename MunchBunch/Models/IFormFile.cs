// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;

// namespace MunchBunch.Models
// {
//     public interface IFormFile
//     {
//         string ContentType { get; }
//         string ContentDisposition { get; }
//         IHeaderDictionary Headers { get; }
//         long Length { get; }
//         string Name { get; }
//         string FileName { get; }
//         Stream OpenReadStream();
//         void CopyTo(Stream target);
//         Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken));
//     }
// }