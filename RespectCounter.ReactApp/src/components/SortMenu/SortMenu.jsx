import './SortMenu.css';
import { OptionsSection } from './OptionsSection/OptionsSection';

function SortMenu({page, onSortOptionChange, onScopeChange}) {

    return (
        <div className="sort-menu">
            <section>
                <h5 className='m-3'>Sort</h5> 
                <OptionsSection page={page} onSortOptionChangeClick={onSortOptionChange} />
            </section>
            {page !== "Comments" &&
                <>
                    <hr/>
                    <section>
                        <ul>
                            <li>
                                <label>Show only verified</label>
                                <input className='m-3' type='checkbox' defaultChecked={true} onClick={(event) => onScopeChange()}/>
                            </li>
                        </ul>
                    </section>
                </>
            }
        </div>
    );
};

export default SortMenu;