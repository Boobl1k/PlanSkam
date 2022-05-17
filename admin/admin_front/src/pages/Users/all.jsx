import {Component} from "react";

export default class aaa extends Component {
    constructor(props) {
        super(props);

        this.state = {
            users: [{Id: ''}]
        }
        this.click = this.click.bind(this);
    }

    click() {
        fetch("http://localhost:3000/users/getAll")
            .then(res => {
                res.text().then(text => this.setState({users: JSON.parse(text)}));
            })
    }

    render() {
        const renderUsers = (users) => {
            return users.map((user, i) => {
                return <div>
                    <p>{user.Id}</p>
                </div>
            })
        }

        return <div>
            {renderUsers(this.state.users)}
            <button onClick={this.click}>button</button>
        </div>
    }
}
