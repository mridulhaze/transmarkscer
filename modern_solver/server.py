# NU H-Type Solver - Local HTTP Server with Auto Port Detection
import http.server
import socketserver
import os
import socket

DEFAULT_PORT = 8085
DIRECTORY = os.path.dirname(os.path.abspath(__file__))

def get_free_port(default_port=8085):
    for port in range(default_port, default_port + 20):
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            if s.connect_ex(('localhost', port)) != 0:
                return port
    return default_port

PORT = get_free_port()

class Handler(http.server.SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=DIRECTORY, **kwargs)

if __name__ == "__main__":
    print(f"Starting NU H-Type Solver Modern Platform at http://localhost:{PORT}")
    with socketserver.TCPServer(("", PORT), Handler) as httpd:
        httpd.serve_forever()
