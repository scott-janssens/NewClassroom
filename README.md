# NewClassroom Web API Demo

The API is running at: www.scottj.net/newclassroom/api/userstats

The following verbs are supported:

### GET
Fetches users from randomuser.me and returns the statistics.

##### Parameters:
<span style="font-family:monospace">users</span>: The number of users to fetch from randomuser.me.

##### Example:
http://www.scottj.net/newclassroom/api/userstats?users=500

### PUT and POST
Returns user statistics for requests containing a randomuser.me payload.

##### Parameters:
<span style="font-family:monospace">None</span>

## Supported Response Formats
The Demo API responds to the Accept header value in the request.  The following formats are supported:
<ul>
<li>applicaiton/json</li>
<li>application/xml</li>
<li>text/json</li>
<li>text/xml</li>
<li>text/plain</li>
</ul>
