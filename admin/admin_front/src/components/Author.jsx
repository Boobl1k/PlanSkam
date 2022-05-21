import {Component} from "react";

export default class Author extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: props.name
        }
        this.changeName = this.changeName.bind(this);
    }

    changeName(){
        const req = new XMLHttpRequest();
        req.open('POST', `http://localhost:3000/authors/changeName?id=${this.props.id}&name=${this.state.name}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4)
                console.log(req.responseText);
        });
        req.send();
    }
    
    render() {
        return (
            <div className="author">
                {this.props.id}
                <br/>
                Name: <input type="text" value={this.state.name} onChange={e => {
                this.setState({name: e.target.value});
            }}/>
                <button onClick={this.changeName}>
                    Change name
                </button>
            </div>
        );
    }
}