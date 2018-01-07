from slackclient import SlackClient
import sys

message = sys.argv[1]

slack_token = 'xoxb-293848887825-1yRvDAVDq2WW0lKxYzcUVfBG'
sc = SlackClient(slack_token)

sc.api_call(
	"chat.postMessage",
	channel="#trader",
	text=message
)