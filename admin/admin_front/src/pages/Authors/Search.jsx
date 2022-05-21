import {Component} from "react";
import Author from "../../components/Author";

export default class Search extends Component {
    constructor(props) {
        super(props);
        this.state = {
            query: "",
            authors: []
        }
        this.search = this.search.bind(this);
    }

    search() {
        if (this.state.query !== '')
            fetch(`http://localhost:3000/authors/search?query=${this.state.query}`)
                .then(res => res.text().then(text => {
                    console.log(text);
                    this.setState({authors: JSON.parse(text)});
                }))
    }

    render() {
        return <div>
            <input type="text" value={this.state.query} onChange={e => {
                this.setState({query: e.target.value});
            }}/>
            <button onClick={this.search}>search</button>
            {this.state.authors.map(author => {
                return <Author id={author.Id} name={author.Name}/>
            })}
        </div>
    }
}