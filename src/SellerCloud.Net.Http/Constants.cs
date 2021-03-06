﻿using System.Net;

namespace SellerCloud.Net.Http
{
    public static class Constants
    {
        public const HttpStatusCode UnknownHttpStatusCode = default;

        public const string UnknownError = "Unknown error";
        public const string NoContentError = "No content";

        public const string ApplicationBinary = "application/octet-stream";
        public const string ApplicationJson = "application/json";
        public const string ApplicationProblemJson = "application/problem+json";
        public const string TextPlain = "text/plain";
        public const string TextHtml = "text/html";
    }
}
