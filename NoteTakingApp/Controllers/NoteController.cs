using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Models;
using System.Diagnostics;
using Ganss.Xss;

namespace NoteTakingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        public class AddRequestBody
        {
            public string Content { get; set; } = "";
        }

        public class GetRequestBody
        {
            public int Id { get; set; } = -1;
        }

        public class EditRequestBody
        {
            public int Id { get; set; } = -1;
            public string Content { get; set; } = "";
        }

        private readonly ILogger<NoteController> _logger;

        private readonly INoteListModel _noteList;
        private readonly INoteModelFactory _noteFactory;
        private readonly HtmlSanitizer _htmlSanitizer;

        public NoteController(ILogger<NoteController> logger, INoteListModel _noteListInstance, INoteModelFactory noteFactory)
        {
            _logger = logger;
            _noteList = _noteListInstance;
            _htmlSanitizer = new HtmlSanitizer();
            _noteFactory = noteFactory;
        }

        /// <summary>
        /// Creates a new Note object in the NoteList collection using the provided Note content.
        /// </summary>
        /// <param name="request">A request object containing the actual content of the note.</param>
        /// <returns>A Response with HTTP Status code 201 to indicate successfully creating a new resource.</returns>
        [HttpPost("Add")]
        public IActionResult AddNote([FromForm] AddRequestBody request)
        {
            // Third-Party HTML Sanitizer mit Standardconfig
            // Bei einem Produkt sollte erlaubter Input konkret spezifiziert sein.
            // Bspw. Markdown Support?
            string sanitizedNoteContext = _htmlSanitizer.Sanitize(request.Content);

            INoteModel newNote = _noteFactory.Create(GetNextId(), sanitizedNoteContext);
            _noteList.Add(newNote.Id, newNote);

            // Using predefined Action for 201 HTTP Response
            return CreatedAtAction(nameof(AddNote), newNote);
        }

        /// <summary>
        /// Returns the properties of a Note, specified by its ID.
        /// </summary>
        /// <param name="request">A request object containing the ID of the note in the collection.</param>
        /// <returns>The specified Note's JSON-formatted properties if it exists, a 404 Response otherwise.</returns>
        [HttpGet("Get")]
        public IActionResult GetNote([FromForm] GetRequestBody request)
        {
            // Specified ID not found in collection. Return corresponding HTTP response code.
            if (!_noteList.ContainsId(request.Id))
            {
                return NotFound();
            }

            // Serialize requested note properties to send over HTTP.
            return Ok(_noteList.Get(request.Id));
        }

        /// <summary>
        /// Returns a list of all existing Notes' properties in the NoteList collection.
        /// </summary>
        /// <returns>A JSON-formatted list of all Notes' properties.</returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllNotes()
        {
            return Ok(_noteList.Notes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public IActionResult EditNote([FromForm] EditRequestBody request)
        {
            // Specified ID not found in collection. Return corresponding HTTP response code.
            if (!_noteList.ContainsId(request.Id))
            {
                return NotFound();
            }

            string sanitizedNoteContext = _htmlSanitizer.Sanitize(request.Content);
            _noteList.Get(request.Id).Content = sanitizedNoteContext;

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteNote([FromForm] GetRequestBody request)
        {
            // Specified ID not found in collection. Return corresponding HTTP response code.
            if (!_noteList.ContainsId(request.Id))
            {
                return NotFound();
            }

            return _noteList.Remove(request.Id) ? Ok() : NotFound();
        }

        /// <summary>
        /// Find the next unused numeric ID in the NoteList collection
        /// </summary>
        /// <returns>An unused numeric ID</returns>
        private int GetNextId()
        {
            int newId = _noteList.NoteCount;

            while (_noteList.ContainsId(newId))
            {
                newId++;
            }

            return newId;
        }
    }
}