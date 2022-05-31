import {Component} from "react";
import Track from "../../components/Track";
import axios from "../../services/api/axios";


export default class Search extends Component {
    constructor(props) {
        super(props);
        this.state = {
            query: "",
            tracks: []
        }
        this.search = this.search.bind(this);
        this.delete = this.delete.bind(this);
    }

    search() {
        if (this.state.query !== '')
            axios.get(`/tracks/searchTracks?query=${this.state.query}`)
                .then(res => {
                    console.log(res.data);
                    this.setState({tracks: res.data});
                })
    }

    delete(trackId) {
        const tracks = this.state.tracks;
        const track = tracks.find(track => track.Id = trackId);
        tracks.splice(tracks.indexOf(track), 1);
        this.setState({tracks: tracks});
    }

    render() {
        return <div>
            <input type="text" value={this.state.query} onChange={e => {
                this.setState({query: e.target.value});
            }}/>
            <button onClick={this.search}>search</button>
            {this.state.tracks.map(track => {
                return <Track id={track.Id} name={track.Name} delete={this.delete}/>
            })}
        </div>
    }
}