from slackclient import SlackClient
import sys

message = sys.argv[1]

slack_token = ''
sc = SlackClient(slack_token)

sc.api_call(
	"chat.postMessage",
	channel="#trader",
	text=message
)