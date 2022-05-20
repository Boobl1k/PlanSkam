import {Component} from "react";

export default class User extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: {},
            isAuthor: false
        }
        fetch(`http://localhost:3000/users?id=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({user: JSON.parse(text)});
            }))
        fetch(`http://localhost:3000/users/isAuthor?id=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({isAuthor: text === 'true'});
            }))
        this.changeAuthorState = this.changeAuthorState.bind(this);
        this.changeEmail = this.changeEmail.bind(this);
    }

    changeAuthorState() {
        const isAuthor = this.state.isAuthor;
        const req = new XMLHttpRequest();
        const method = isAuthor ? "makeNotAuthor" : "makeAuthor"
        req.open('POST', `http://localhost:3000/users/` + method + `?id=${this.state.user.Id}`);
        req.send();
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4)
                console.log(req.responseText);
            if (req.readyState === 4 && req.status === 201)
                this.setState({isAuthor: !isAuthor});
        });
    }

    changeEmail() {
        const req = new XMLHttpRequest();
        req.open('POST', `http://localhost:3000/users/changeEmail?id=${this.state.user.Id}&email=${this.state.user.Email}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4)
                console.log(req.responseText);
        });
        req.send();
    }

    render() {
        return (
            <div>
                <p>Id: {this.state.user.Id}</p>
                <p>UserName: {this.state.user.UserName}</p>
                <p>{this.state.isAuthor ? 'Author' : 'Not author'}</p>
                <button onClick={this.changeAuthorState}>
                    {this.state.isAuthor ? 'Make not author' : 'Make author'}
                </button>
                <br/>
                <input type="text" name="email" value={this.state.user.Email} onChange={e => {
                    const user = this.state.user;
                    user.Email = e.target.value;
                    this.setState({user});
                }}/>
                <br/>
                <button onClick={this.changeEmail}>Change email</button>
            </div>
        );
    }
}