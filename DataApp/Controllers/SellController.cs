using DataApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
namespace DataApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SellController : Controller
    {
        string mongoConnection = "mongodb://mongouser:mongo123@sp21-cs411-26.cs.illinois.edu:27017/?authSource=carsmongodb&readPreference=primary&appname=MongoDB%20Compass&ssl=false";
        string connection = "SERVER=127.0.0.1;PORT=3306;UID=root;DATABASE=Main";

        /// <summary>
        /// Addes a new questions to a listing. If listing does not exist yet in the mongo db, it will create a new document for it
        /// </summary>
        /// <param name="listingId"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{listingId}")]
        public QuestionObject PostQuestion(long listingId, string question)
        {
            var conn = new MongoClient(mongoConnection);
            var db = conn.GetDatabase("carsmongodb");
            var questionsCollection = db.GetCollection<QuestionDocument>("qna");

            // mongodb shell command: {find({listingid:listingId})
            var listing = questionsCollection.Find(q => q.ListingId == listingId).FirstOrDefault();
            long maxNextId = 1;

            if (listing == null)
            {
                listing = new QuestionDocument
                {
                    ListingId = listingId
                };
            }
            else
            {
                maxNextId = listing.Questions.Max(q => q.QuestionId) + 1;
            }

            var newQuestion = new QuestionObject
            {

                QuestionId = maxNextId,
                Question = question,
                Replies = new List<string>()
            };

            listing.Questions.Add(newQuestion);

            if (maxNextId == 1)
            {
                questionsCollection.InsertOne(listing);

            }
            else
            {
                questionsCollection.ReplaceOne(filter: l => l.ListingId == listingId, listing, new ReplaceOptions());
            }

            // retrieve and return latest inserted question document

            return questionsCollection
                .Find(q => q.ListingId == listingId)
                .First().Questions.First(q => q.QuestionId == maxNextId);
        }

        /// <summary>
        /// Addes a reply to a question on a listing
        /// </summary>
        /// <param name="listingId"></param>
        /// <param name="questionId"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{listingId}/{questionId}")]

        public QuestionObject PostReply(long listingId, long questionId, string reply)
        {
            var conn = new MongoClient(mongoConnection);
            var db = conn.GetDatabase("carsmongodb");
            var questionsCollection = db.GetCollection<QuestionDocument>("qna");
            var listing = questionsCollection.Find(q => q.ListingId == listingId).FirstOrDefault();

            if (listing == null)
            {
                return null;
            }

            var question = listing.Questions.FirstOrDefault(q => q.QuestionId == questionId);

            if (question == null)
            {
                return null;
            }

            question.Replies.Add(reply);

            questionsCollection.ReplaceOne(filter: l => l.ListingId == listingId, listing, new ReplaceOptions());

            return questionsCollection
                .Find(q => q.ListingId == listingId)
                .First().Questions.First(q => q.QuestionId == questionId);

        }

        /// <summary>
        /// Display all questions and their replies for a listing
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{listingId}")]
        public QuestionDocument Get(long listingId)
        {
            var conn = new MongoClient(mongoConnection);
            var db = conn.GetDatabase("carsmongodb");
            var questionsCollection = db.GetCollection<QuestionDocument>("qna");
            return questionsCollection.Find(q => q.ListingId == listingId).FirstOrDefault();
        }
    }
}
